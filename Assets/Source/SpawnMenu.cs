using System.Collections.Generic;
using Source.Turrets;
using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class SpawnMenu : MonoBehaviour
    {
        public List<turretSpawn> blue;
        public GameObject blueButton;
        public GameObject blueTurret;
        private Button blueTurretButton;

        private int blueStage = 0;

        public List<turretSpawn> red;
        public GameObject redButton;
        public GameObject redTurret;
        private Button redTurretButton;

        private int redStage = 0;


        public List<turretSpawn> yellow;
        public GameObject yellowButton;
        public GameObject yellowTurret;
        private Button yellowTurretButton;

        private int yellowStage = 0;


        public GameManger GameManger;

        private void Start()
        {
            blueStage = 0;
            redStage = 0;
            yellowStage = 0;

            redTurretButton = redButton.GetComponent<Button>();
            blueTurretButton = blueButton.GetComponent<Button>();
            yellowTurretButton = yellowButton.GetComponent<Button>();

            updateButtons();
        }


        private void Update()
        {
            blueButton.SetActive(blueStage != -1);
            redButton.SetActive(redStage != -1);
            yellowButton.SetActive(yellowStage != -1);

            redTurretButton.interactable = GameManger.canMoveToNextStage;
            blueTurretButton.interactable = GameManger.canMoveToNextStage;
            yellowTurretButton.interactable = GameManger.canMoveToNextStage;
        }

        public void addBlueStage()
        {
            if (blueStage == 8) return;
            if (blue[blueStage + 1].unlockAtStage > GameManger.stage) return;
            blueStage++;
            updateButtons();
        }

        public void subBlueStage()
        {
            if (blueStage - 1 < 0) return;
            blueStage--;
            updateButtons();
        }

        public void addRedStage()
        {
            if (redStage == 8) return;
            if (red[redStage + 1].unlockAtStage > GameManger.stage) return;
            redStage++;
            updateButtons();
        }

        public void subRedStage()
        {
            if (redStage - 1 < 0) return;
            redStage--;
            updateButtons();
        }

        public void addYellowStage()
        {
            if (yellowStage == 8) return;
            if (yellow[yellowStage + 1].unlockAtStage > GameManger.stage) return;
            yellowStage++;
            updateButtons();
        }

        public void subYellowStage()
        {
            if (yellowStage - 1 < 0) return;
            yellowStage--;
            updateButtons();
        }

        private void updateButtons()
        {
            if (blue[0].unlockAtStage <= GameManger.stage)
            {
                blueButton.GetComponent<Image>().sprite = blue[blueStage].Sprite;
                var blueTexts = blueButton.GetComponentsInChildren<Text>();
                blueTexts[0].text = RomanNumeralsUtils.ToRoman(blueStage + 1);
                blueTexts[1].text = blue[blueStage].price + " $";
            }


            if (red[0].unlockAtStage <= GameManger.stage)
            {
                redButton.GetComponent<Image>().sprite = red[redStage].Sprite;
                var redTexts = redButton.GetComponentsInChildren<Text>();
                redTexts[0].text = RomanNumeralsUtils.ToRoman(redStage + 1);
                redTexts[1].text = red[redStage].price + " $";
            }


            if (yellow[0].unlockAtStage <= GameManger.stage)
            {
                yellowButton.GetComponent<Image>().sprite = yellow[yellowStage].Sprite;
                var yellowTexts = yellowButton.GetComponentsInChildren<Text>();
                yellowTexts[0].text = RomanNumeralsUtils.ToRoman(yellowStage + 1);
                yellowTexts[1].text = yellow[yellowStage].price + " $";
            }
        }

        private Vector2 findNextSpawnLocation()
        {
            for (int i = 8; i >= -7; i--)
            {
                for (int j = 4; j >= -4; j--)
                {
                    var hit = Physics2D.OverlapBox(new Vector2(i, j), new Vector2(0.5f, 0.5f), 0);
                    if (!hit)
                    {
                        return new Vector2(i, j);
                    }
                }
            }

            return Vector2.zero;
        }


        public void spawnBlue()
        {
            if (GameManger.money < blue[blueStage].price) return;
            var placement = findNextSpawnLocation();
            if (placement != Vector2.zero)
            {
                var turret = Instantiate(blueTurret, placement, Quaternion.identity);
                GameManger.money -= blue[blueStage].price;
                turret.GetComponent<Turret>().level = blueStage;
            }
        }

        public void spawnRed()
        {
            if (GameManger.money < red[redStage].price) return;
            var placement = findNextSpawnLocation();
            if (placement != Vector2.zero)
            {
                var turret = Instantiate(redTurret, placement, Quaternion.identity);
                GameManger.money -= red[redStage].price;
                turret.GetComponent<Turret>().level = redStage;
            }
        }

        public void spawnYellow()
        {
            if (GameManger.money < yellow[yellowStage].price) return;
            var placement = findNextSpawnLocation();
            if (placement != Vector2.zero)
            {
                var turret = Instantiate(yellowTurret, placement, Quaternion.identity);
                GameManger.money -= yellow[yellowStage].price;
                turret.GetComponent<Turret>().level = yellowStage;
            }
        }
    }
}