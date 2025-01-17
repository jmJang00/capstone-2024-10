using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Fusion;
using UnityEngine;

public class ObjectManager
{
	public Dictionary<NetworkId, Crew> Crews { get; protected set; }
    public Dictionary<NetworkId, Alien> Aliens { get; protected set; }

    public Creature MyCreature { get; set; }
    public Transform CrewRoot => GetRootTransform("@Crews");
    public Transform AlienRoot => GetRootTransform("@Aliens");

    public void Init()
    {
	    Crews = new Dictionary<NetworkId, Crew>();
	    Aliens = new Dictionary<NetworkId, Alien>();
    }

    public Transform GetRootTransform(string name)
    {
	    GameObject root = GameObject.Find(name);
	    if (root == null)
		    root = new GameObject { name = name };

	    return root.transform;
    }

    public NetworkObject SpawnCrew(int crewDataId, Vector3 spawnPosition)
    {
        string className = Managers.DataMng.CrewDataDict[crewDataId].Name;
        NetworkObject prefab = Managers.ResourceMng.Load<NetworkObject>($"{Define.CREW_PATH}/{className}");
        NetworkObject no = Managers.NetworkMng.Runner.Spawn(prefab, spawnPosition);

        Crew crew = no.GetComponent<Crew>();
        crew.SetInfo(crewDataId);

        return no;
    }

    public NetworkObject SpawnAlien(int alienDataId, Vector3 spawnPosition)
    {
        string className = Managers.DataMng.AlienDataDict[alienDataId].Name;
        NetworkObject prefab = Managers.ResourceMng.Load<NetworkObject>($"{Define.ALIEN_PATH}/{className}");
        NetworkObject no = Managers.NetworkMng.Runner.Spawn(prefab, spawnPosition);

        Alien alien = no.GetComponent<Alien>();
        alien.SetInfo(alienDataId);

        return no;
    }

    public void Despawn(NetworkObject no)
    {
        Creature creature = no.GetComponent<Creature>();
        if (creature.CreatureType == Define.CreatureType.Crew)
            Crews.Remove(no.Id);
        else if (creature.CreatureType == Define.CreatureType.Alien)
            Aliens.Remove(no.Id);
        else
        {
            Debug.Log("Invalid Despawn");
            return;
        }

        Managers.NetworkMng.Runner.Despawn(no);
    }

    public CreatureData GetCreatureDataWithDataId(int dataId)
    {
	    if (Managers.DataMng.CrewDataDict.TryGetValue(dataId, out CrewData crewData))
		    return crewData;
	    if (Managers.DataMng.AlienDataDict.TryGetValue(dataId, out AlienData alienData))
            return alienData;

        Debug.Log("Invalid GetCreatureDataWithDataId");
	    return null;
    }
}
