using Dun9eonAndFi9ht.StaticClass;
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
    }
    internal class Scene
    {
        public virtual void Start()
        {
            Utility.ClearScene();
        }
    }
}
