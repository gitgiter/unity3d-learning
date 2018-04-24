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
        void ActionDone(SSAction source);
    }

    public interface ISceneControl
    {
        void LoadPrefabs();
        void PlayDisk();
    }

    public interface IUserAction
    {
        void Begin();
        void Hit(DiskControl diskCtrl);
        void Restart();
        void SwitchMode();
    }

    public class DiskControl : MonoBehaviour
    {
        public float size;
        public Color color;
        public float speed;
        public bool hit = false;
        public SSAction action;
    }

    public class RoundControl
    {
        int round = 0;
        public float scale;
        public Color color;
        public RoundControl(int r)
        {
            round = r;
            scale = 5 - r;
            switch (r)
            {
                case 1:
                    color = Color.blue;
                    break;
                case 2:
                    color = Color.red;
                    break;
                case 3:
                    color = Color.yellow;
                    break;
            }            
        }
    }

    public class RecordControl : MonoBehaviour
    {
        public int Score = 0;//分数
        public FirstControl sceneControler { get; set; }
        // Use this for initialization
        void Start()
        {
            sceneControler = (FirstControl)Director.getInstance().sceneCtrl;
            sceneControler.scoreRecorder = this;
        }
        public void add()
        {
            Score += sceneControler.user.round;
            sceneControler.user.score = Score;
            //Debug.Log(Score);
        }
        public void miss()
        {
            Score -= sceneControler.user.round;
            sceneControler.user.score = Score;
            //Debug.Log(Score);
        }
    }
}