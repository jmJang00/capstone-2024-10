using System.Collections.Generic;
using UnityEngine;
using Data;

public abstract class Alien : Creature
{
    #region Field

    public AlienData AlienData => CreatureData as AlienData;
    public AlienStat AlienStat => (AlienStat)BaseStat;

    public List<BaseSkill> Skills { get; protected set; }

    #endregion

    public override void SetInfo(int templateID)
    {
        CreatureType = Define.CreatureType.Alien;
        Transform.parent = Managers.ObjectMng.AlienRoot;

        base.SetInfo(templateID);

        AlienStat.SetStat(AlienData);

        Skills = new List<BaseSkill>();
    }

    protected override void HandleInput()
    {
        base.HandleInput();

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (CheckInteract(false))
            {
                CreatureState = Define.CreatureState.Interact;
                return;
            }
        }
        else
            CheckInteract(true);

        if (Input.GetMouseButtonDown(0))
        {
            if (CheckAndUseSkill(0))
            {
                CreatureState = Define.CreatureState.Use;
                return;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (CheckAndUseSkill(1))
            {
                CreatureState = Define.CreatureState.Use;
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (CheckAndUseSkill(2))
            {
                CreatureState = Define.CreatureState.Use;
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CheckAndUseSkill(3))
            {
                CreatureState = Define.CreatureState.Use;
                return;
            }
        }

        if (Velocity == Vector3.zero)
            CreatureState = Define.CreatureState.Idle;
        else
        {
            CreatureState = Define.CreatureState.Move;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                CreaturePose = Define.CreaturePose.Run;
            }
            else
            {
                if (CreaturePose == Define.CreaturePose.Run)
                {
                    CreaturePose = Define.CreaturePose.Stand;
                }
            }
        }
    }

    #region Update

    protected override void UpdateIdle()
    {
        switch (CreaturePose)
        {
            case Define.CreaturePose.Stand:
                break;
            case Define.CreaturePose.Run:
                CreaturePose = Define.CreaturePose.Stand;
                break;
        }

        if (IsFirstPersonView)
        {
            KCC.SetLookRotation(0, CreatureCamera.transform.rotation.eulerAngles.y);
        }
    }

    protected override void UpdateMove()
    {
        switch (CreaturePose)
        {
            case Define.CreaturePose.Stand:
                BaseStat.Speed = AlienData.WalkSpeed;
                break;
            case Define.CreaturePose.Run:
                BaseStat.Speed = AlienData.RunSpeed;
                break;
        }

        if (IsFirstPersonView)
        {
            KCC.SetLookRotation(0, CreatureCamera.transform.rotation.eulerAngles.y);
        }
        else
        {
            if (Velocity != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(Velocity);
                KCC.SetLookRotation(newRotation);
            }
        }

        KCC.Move(Velocity, 0f);
    }

    protected override void UpdateUse()
    {
        //AlienSkill alienSkill = new AlienSkill();
        //alienSkill.Rpc_Use();
    }

    protected override void UpdateDead()
    {
        // TODO
    }

    #endregion

    protected bool CheckAndUseSkill(int skillIdx)
    {
        if (Skills[skillIdx] == null)
        {
            Debug.Log("No SKill" + skillIdx);
            return false;
        }

        return Skills[skillIdx].CheckAndUseSkill();
    }
}
