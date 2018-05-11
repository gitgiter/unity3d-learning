using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Director : System.Object
    {
        private static Director _instance;
        public ISceneControl sceneCtrl { get; set; }

        public bool playing { get; set; } //

        public static Director getInstance()
        {
            if (_instance == null) return _instance = new Director();
            else return _instance;
        }

        public int getFPS()
        {
            return Application.targetFrameRate;
        }

        public void setFPS(int fps)
        {
            Application.targetFrameRate = fps;
        }
    }

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        protected static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    //instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    instance = (T)FindObjectOfType(typeof(T));
                    if (instance == null)
                    {
                        Debug.LogError("no scene instance");
                    }
                }
                return instance;
            }
        }
    }

    public interface ISSActionCallback
    {
        void ActionDone(SSAction source, bool catchState = false);
    }

    public interface ISceneControl
    {
        void LoadPrefabs();
    }

    public interface IUserAction
    {
        void heroMove(int dir);
        void Restart();
    }

    public class Diretion
    {
        public const int UP = 0;
        public const int DOWN = 2;
        public const int LEFT = -1;
        public const int RIGHT = 1;
    }

    public interface IAddAction
    {
        void addRandomMovement(GameObject sourceObj, bool isActive);
        void addDirectMovement(GameObject sourceObj);
    }

    public interface IGameStatusOp
    {
        Transform getHeroPosition();
        void heroEscapeAndScore();
        void patrolHitHeroAndGameover();
    }
}