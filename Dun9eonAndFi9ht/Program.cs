using Dun9eonAndFi9ht.Scenes;

namespace Dun9eonAndFi9ht
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            gameManager.LoadScene(ESceneType.StartScene);
        }
    }
}
