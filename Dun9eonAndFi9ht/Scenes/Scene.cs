using DataDefinition;
using Dun9eonAndFi9ht.StaticClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dun9eonAndFi9ht.Scenes
{
    internal class Scene
    {
        public virtual ESceneType Start()
        {
            Utility.ClearScene();
            Utility.ClearMenu();
            return ESceneType.StartScene;
        }
    }
}
