namespace DungeonSpawner
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    public class GUICellularAutomata : MonoBehaviour
    {
        private bool _lastIsGeneric = true;
        private CellularAutomata _automata;
        private HexCellularAutomata _hexAutomata;
        public DungeonSpawner DungeonSpawner;


        public Slider GlobalPass;
        public Camera MainCamera;
        public InputField Width;
        public InputField Height;
        public InputField Seed;

        public Slider Type1Pass;
        public Slider Type2Pass;
        public Slider Type3Pass;
        public Slider Type4Pass;

        void Awake()
        {
            _automata = new CellularAutomata(100, 100, new System.Random());
            _hexAutomata = new HexCellularAutomata(100, 100, new System.Random());
            _automata.Player = DungeonSpawner.Player;

            Height.text = "100";
            Width.text = "100";
            Seed.text = "random";
        }
        public void Exit()
        {
            Application.Quit();
        }

        public void GenerateBasic()
        {
            _automata.Destroy();
            Scene.SpawnScene();
            /*_automata.RandomFillMap();
            _automata.ProcessCavern((int)GlobalPass.value);
            _automata.PrintMap();*/
            _lastIsGeneric = true;
        }

        public void NewAutomata()
        {
            _automata.Destroy();
            if(Seed.text.ToLower() == "random")
                _automata = new CellularAutomata(int.Parse(Width.text), int.Parse(Height.text), new System.Random());
            else
                _automata = new CellularAutomata(int.Parse(Width.text), int.Parse(Height.text), new System.Random());
            _automata.Player = DungeonSpawner.Player;
        }

        public void OnGlobalPassChanged()
        {
            GlobalPass.transform.FindChild("Text").GetComponent<Text>().text = string.Format("Nombre de passes: {0}", GlobalPass.value);
        }

        public void Delete()
        {
            _automata.Destroy();
        }

        public void Type1()
        {
            CheckIsLastGeneric();
            _automata.PlaceWalls_1D5678((int)Type1Pass.value);
            _automata.ProcessBorders();
            _automata.PrintMap();
        }

        public void OnType1PassChanged()
        {
            Type1Pass.transform.FindChild("Text").GetComponent<Text>().text = string.Format("Nombre de passes: {0}", Type1Pass.value);
        }

        public void Type2()
        {
            CheckIsLastGeneric();
            _automata.PlaceWalls_1D5678_2D1((int)Type2Pass.value);
            _automata.ProcessBorders();
            _automata.PrintMap();
        }

        public void OnType2PassChanged()
        {
            Type2Pass.transform.FindChild("Text").GetComponent<Text>().text = string.Format("Nombre de passes: {0}", Type2Pass.value);
        }

        public void Type3()
        {
            CheckIsLastGeneric();
            _automata.PlaceWalls_1D5678_2D12((int)Type3Pass.value);
            _automata.ProcessBorders();
            _automata.PrintMap();
        }

        public void OnType3PassChanged()
        {
            Type3Pass.transform.FindChild("Text").GetComponent<Text>().text = string.Format("Nombre de passes: {0}", Type3Pass.value);
        }

        public void Type4()
        {
            CheckIsLastGeneric();
            _hexAutomata.RandomFillMap();
            _hexAutomata.ProcessCavern(3);
            //_hexAutomata.ProcessBorders();
            _hexAutomata.PrintMap();
            //_automata.PlaceWalls_1B345678((int)Type4Pass.value);
            //_automata.ProcessBorders();
            //_automata.PrintMap();
        }

        public void PlayGame()
        {

        }

        public void OnType4PassChanged()
        {
            Type4Pass.transform.FindChild("Text").GetComponent<Text>().text = string.Format("Nombre de passes: {0}", Type4Pass.value);
        }

        private void CheckIsLastGeneric()
        {
            _automata.Destroy();
            if (_lastIsGeneric)
            {
                _automata.RandomFillMap();
                _lastIsGeneric = false;
            }
        }

        void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                MainCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 5;
            }
            if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                MainCamera.transform.position = new Vector3(
                    MainCamera.transform.position.x + (2 * Input.GetAxis("Horizontal")),
                    MainCamera.transform.position.y + (2 * Input.GetAxis("Vertical")),
                    -50f);
            }
        }
    }

}