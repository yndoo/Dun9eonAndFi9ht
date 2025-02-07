using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dun9eonAndFi9ht.Characters;
using Dun9eonAndFi9ht.Scenes;

namespace Dun9eonAndFi9ht.Manager
{

    internal class GameManager
    {
        private Player player;
        private List<Scene> sceneList;
        public List<Scene> SceneList { get; set; }
        public Player Player { get; set; }

        public GameManager()
        {
            player = new Player();
            sceneList = new List<Scene>();
            sceneList.Add(new StartScene());
        }
        public void LoadScene(ESceneType type)
        {
            sceneList[(int)type].Start();
        }

        public void LoadScene(int type)
        {
            sceneList[type].Start();
        }
    }
}
