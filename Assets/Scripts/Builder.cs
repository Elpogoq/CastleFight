﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Builder : Selectable {
    const int RIGHT_CLICK = 1;

    public List<GameObject> buildings;
    public float incomeTime;

    List<GameObject> ownedBuildings;
    float cooldown;
    uint money;
    float income;

    NavMeshAgent navMesh;
    Collider planeCollider;
    Vector3 destination;


    public bool EnoughMoney(uint cost)
    {
        return cost < money;
    }

    public void LosteMoney(uint cost)
    {
        money -= cost;
    }

    new private void Start()
    {
        base.Start();
        this.uiPanel.GetComponent<UIBuilderManager>().builder = this;
        planeCollider = GameObject.FindGameObjectWithTag("Plane").GetComponent<Collider>();
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.speed = 1000;
        uiPanel.GetComponent<UIBuilderManager>();
        this.ownedBuildings = new List<GameObject>();
        this.money = 100;
        this.income = 10f;
        this.cooldown = incomeTime;
        destination = this.transform.position;
    }

    protected void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown < 0)
        {
            cooldown = incomeTime; // Observer pattern for income?
            UpdateMoney();
        }
    }

    void GetDestination()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (planeCollider.Raycast(ray, out hitInfo, 1000f))
        {
            destination = hitInfo.point;
        }
    }

    void Move(Vector3 position)
    {
        navMesh.SetDestination(position);
    }

    public GameObject InstantiateBuilding(GameObject building)
    {
        GameObject b = Instantiate(building);
        ownedBuildings.Add(b);
        Building buildingComponent = b.GetComponent<Building>();
        buildingComponent.creator = this;
        buildingComponent.AdjustStart();
        income += b.GetComponent<Building>().income;
        return b;
    }

    public void DestroyBuilding(GameObject building)
    {
        ownedBuildings.Remove(building);
        Destroy(building);
        income -= ownedBuildings[ownedBuildings.Count - 1].GetComponent<Building>().income;
    }

    void UpdateMoney()
    {
        money += (uint)income;
    }
}
