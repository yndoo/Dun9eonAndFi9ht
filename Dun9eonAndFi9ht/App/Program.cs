﻿using Dun9eonAndFi9ht.Manager;
using Dun9eonAndFi9ht.Scenes;

namespace Dun9eonAndFi9ht.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameManager.Instance.LoadScene(ESceneType.StartScene);
        }
    }
}
