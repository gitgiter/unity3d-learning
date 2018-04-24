using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class ActionManagerAdapter
{
    ActionManager normalAM;
    PhysicsActionManager PhysicsAM;

    public int mode = 0; // 0->normal, 1->physics

    public ActionManagerAdapter(GameObject main)
    {
        normalAM = main.AddComponent<ActionManager>();
        PhysicsAM = main.AddComponent<PhysicsActionManager>();
        mode = 0;
    }

    public void SwitchActionMode()
    {
        mode = 1 - mode;
    }

    public void PlayDisk(int round)
    {
        if (mode == 0)
        {
            Debug.Log("normalAM");
            normalAM.playDisk(round);
        }
        else
        {
            Debug.Log("physicsAM");
            PhysicsAM.playDisk(round);
        }
    }

    public void SetNormalAM(ActionManager am)
    {
        normalAM = am;
    }

    public void SetPhysicsAM(PhysicsActionManager pam)
    {
        PhysicsAM = pam;
    }

    public ActionManager GetNormalAM()
    {
        return normalAM;
    }

    public PhysicsActionManager GetPhysicsAM()
    {
        return PhysicsAM;
    }
}