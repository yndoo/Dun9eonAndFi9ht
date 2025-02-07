using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Scenes
{
    enum ESceneType
    {
        StartScene,
        Dungeon,
        ResultScene,
    }
    internal class Scene
    {
        public virtual void Start()
        {
            Console.Clear();
        }
    }
}
