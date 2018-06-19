using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    private const float ydis = 0.5f;//y间隔    
    private float[] ypos;//y位置    
    private const float zdis = 2;//z间隔    
    private float[] zpos;//z位置
    //private GameObject[] barriers;//所有障碍物
    private Queue<GameObject> barriers;//所有障碍物
    private float delta;//移动距离

    // Use this for initialization
    void Start () {
        //初始化队列
        barriers = new Queue<GameObject>();     

        //初始化y位置
        ypos = new float[3];
        for (int i = 0; i < 3; i++)
        {
            ypos[i] = ydis * i + 0.3f; //0.3是偏移
        }

        //初始化z位置
        zpos = new float[5];
        for (int i = 0; i < 5; i++)
        {            
            zpos[i] = zdis * (i + 1f); //保证障碍物全在视野外
        }

        Debug.Log(ypos);
        Debug.Log(zpos);

        //移入视野前产生5列障碍物，确保移动过程中能无缝衔接
        for (int i = 0; i < 5; i++)
        {
            int count = 0; //每列的障碍物数量

            //每列的三个位置
            bool[] state = { false, false, false };
            for (int j = 0; j < 3; j++)
            {
                //随机一个位置
                int rpos = (int)Random.Range(0, 3);
                //该位置必须没有放置过
                while (state[rpos])
                {
                    rpos = (int)Random.Range(0, 3);
                }
                state[rpos] = true;

                //在该位置随机产生或不产生障碍物
                float rand = Random.Range(0, 1);
                if (rand < 0.5)
                {
                    //加载预制障碍物
                    GameObject barrier = (GameObject)Instantiate(Resources.Load("Barrier", typeof(GameObject)), 
                        new Vector3(0, ypos[rpos], zpos[i]), Quaternion.identity, null);                    
                    barrier.transform.parent = this.transform;
                    barrier.transform.position *= 0.03f;
                    barrier.transform.localScale *= 0.03f;
                    barrier.GetComponent<BoxCollider>().center = barrier.transform.position;
                    barriers.Enqueue(barrier);
                    count++;
                }

                //每列的障碍物不能超过2个
                if (count >= 2) break;
            }
        }

        //初始化距离
        delta = 0;
    }

    // Update is called once per frame
    void Update () {
        //移动
        
        //遍历队列
        int len = barriers.Count;
        while (len > 0)
        {
            //取出第一个
            GameObject front = barriers.Peek();
            front.transform.position -= new Vector3(0, 0, 0.0002f);                       
            barriers.Dequeue();

            //Debug.Log("front pos: " + front.transform.position);

            //消除移出视野的障碍物                       
            if (front.transform.position.z < -1 * 0.03f)
            {
                Destroy(front);
            }
            //否则插回队列尾部
            else
            {
                barriers.Enqueue(front);
            }

            len--;
        }

        //移动距离增加，注意相对坐标和绝对坐标的转换
        delta += 0.0002f / 0.03f;

        //如果移动了一个z间距，那么并产生新一列放在最后 
        if (delta >= zdis)
        {
            Debug.Log(delta);
            int count = 0; //每列的障碍物数量

            //每列的三个位置
            bool[] state = { false, false, false}; 
            for (int j = 0; j < 3; j++)
            {
                //随机一个位置
                int rpos = (int)Random.Range(0, 3);
                //该位置必须没有放置过
                while (state[rpos])
                {
                    rpos = (int)Random.Range(0, 3);
                }
                state[rpos] = true;
                Debug.Log(rpos);

                //在该位置随机产生或不产生障碍物
                float rand = Random.Range(0, 1);
                if (rand < 0.5)
                {
                    //加载预制障碍物
                    GameObject barrier = (GameObject)Instantiate(Resources.Load("Barrier", typeof(GameObject)),
                        new Vector3(0, ypos[rpos], zpos[4]), Quaternion.identity, null);
                    barrier.transform.parent = this.transform;
                    barrier.transform.position *= 0.03f;
                    barrier.transform.localScale *= 0.03f;
                    barrier.GetComponent<BoxCollider>().center = barrier.transform.position;
                    barriers.Enqueue(barrier);
                    //Debug.Log(barrier);
                    count++;
                }

                //每列的障碍物不能超过2个
                if (count >= 2) break;
            }

            delta = 0;
        }

    }    
}
