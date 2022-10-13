using System;
using Source;
using Source.Turrets;
using UnityEngine;
using UnityEngine.UI;


public class DraggableObject : MonoBehaviour
{
    private Vector3 startPos;
    private BoxCollider2D _collider;
    private bool isColliding;
    public LayerMask LayerMask;
    private Turret _turret;
    private GameObject AreaDisplay;

    private GameManger _gameManger;

    public GameObject orangePrefab;
    public GameObject greenPrefab;
    public GameObject purplePrefab;


    private void Start()
    {
        _gameManger = GameObject.FindWithTag("GameManger").GetComponent<GameManger>();
        _collider = gameObject.GetComponent<BoxCollider2D>();
        _turret = gameObject.GetComponent<Turret>();
        AreaDisplay = GameObject.FindWithTag("AreaDisplay");
    }

    private void OnEnable()
    {
        AreaDisplay = GameObject.FindWithTag("AreaDisplay");
    }

    private void OnMouseDown()
    {
        startPos = transform.position;
        AreaDisplay.transform.position = transform.position;
        AreaDisplay.GetComponent<SpriteRenderer>().enabled = true;
        AreaDisplay.GetComponentInChildren<Text>().enabled = true;
        AreaDisplay.GetComponentInChildren<Text>().text = "LEVEL: " + RomanNumeralsUtils.ToRoman(_turret.level + 1);
    }

    private void OnMouseDrag()
    {
        if (_gameManger.canMoveToNextStage)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.localPosition = new Vector3(mousePos.x, mousePos.y, 0);
            AreaDisplay.transform.position = transform.position;
        }

        if (_turret.State == turretState.green)
        {
            AreaDisplay.GetComponent<SpriteRenderer>().size = new Vector2(0, 0);
        }
        else
        {
            AreaDisplay.GetComponent<SpriteRenderer>().size = _turret.GetComponentInChildren<Gun>().range;
        }

        AreaDisplay.GetComponentInChildren<Text>().transform.position = transform.position - new Vector3(0, 0.5f, 0);
    }

    private void OnMouseUp()
    {
        _gameManger.LevelText.text = "LEVEL: ";
        AreaDisplay.GetComponent<SpriteRenderer>().enabled = false;

        AreaDisplay.GetComponentInChildren<Text>().enabled = false;
        gameObject.layer = 0;
        var hit = Physics2D.OverlapBox(transform.position, new Vector2(0.2f, 0.2f), 0, LayerMask);
        if (hit)
        {
            if (hit.tag == "Turret")
            {
                var turret = hit.GetComponent<Turret>();


                if (turret.level < turret.levels.Length &&
                    turret.level == _turret.level)
                {
                    if (turret.State == _turret.State)
                    {
                        turret.level++;
                        turret.updateTurret();
                        Destroy(gameObject);
                        return;
                    }
                    else if ((_turret.State == turretState.blue && turret.State == turretState.red) ||
                             (turret.State == turretState.blue && _turret.State == turretState.red))
                    {
                        var tower = Instantiate(purplePrefab, turret.transform.position, Quaternion.identity)
                            .GetComponent<Turret>();
                        tower.level = turret.level;
                        tower.updateTurret();
                        Destroy(gameObject);
                        Destroy(turret.gameObject);
                    }
                    else if ((_turret.State == turretState.blue && turret.State == turretState.yellow) ||
                             (turret.State == turretState.blue && _turret.State == turretState.yellow))
                    {
                        var tower = Instantiate(greenPrefab, turret.transform.position, Quaternion.identity)
                            .GetComponent<Turret>();
                        tower.level = turret.level;
                        tower.updateTurret();
                        Destroy(gameObject);
                        Destroy(turret.gameObject);
                    }
                    else if ((_turret.State == turretState.red && turret.State == turretState.yellow) ||
                             (turret.State == turretState.red && _turret.State == turretState.yellow))
                    {
                        var tower = Instantiate(orangePrefab, turret.transform.position, Quaternion.identity)
                            .GetComponent<Turret>();
                        tower.level = turret.level;
                        tower.updateTurret();
                        Destroy(gameObject);
                        Destroy(turret.gameObject);
                    }
                }
            }

            transform.position = startPos;
        }
        else
        {
            transform.position = Snap(transform.position);
        }

        gameObject.layer = 6;
    }


    public static Vector3 Snap(Vector3 vector3)
    {
        return new Vector3(
            Mathf.Round(vector3.x),
            Mathf.Round(vector3.y),
            Mathf.Round(vector3.z)
        );
    }
}