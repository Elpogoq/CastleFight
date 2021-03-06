﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

public class MouseManager : MonoBehaviour {
    const int LEFT_CLICK = 0;

    public GameObject defaultSelection;
    public GameObject hoveredObject;
    public GameObject selectedObject;

    public GameObject thrashCode;
    public UIManager uiManager;

    public Color previousHoveredColor;
    public Color previousSelectedColor;

    private GameObject previousSelectedObject;

    const int UI_POINTER = -1;

    // Use this for initialization
    void Start () {
        selectedObject = previousSelectedObject = hoveredObject = defaultSelection;
        UpdatePanel();
    }

    // Update is called once per frame
    void Update () {
        previousSelectedObject = selectedObject;
        //Look if selectedObject was destroyed
        if(selectedObject == null) { selectedObject = defaultSelection; }
        if (EventSystem.current.IsPointerOverGameObject(UI_POINTER)) {
            
        } else {
            LookForHover();
            LookForSelect();
        }
        if(selectedObject != previousSelectedObject)
        {
            UpdatePanel();
        }
    }

    void LookForHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject hitObject = hitInfo.transform.gameObject;
            HoverObject(hitObject);
        }
        else
        {
            Clear(ref hoveredObject);
        }
    }
    
    void LookForSelect()
    {
        if (Input.GetMouseButtonDown(LEFT_CLICK))
        {
            if (selectedObject == hoveredObject)
            {
                return;
            }

            Clear(ref selectedObject);

            Selectable obj = hoveredObject.GetComponent<Selectable>();
            if ( obj != null)
            {
                obj.isSelected = true;
                selectedObject = hoveredObject;
            }
        }
    }

    void HoverObject(GameObject selection)
    {
        hoveredObject = selection;
        Selectable obj = selection.GetComponent<Selectable>();
    }

    void Clear(ref GameObject obj)// Rename?
    {
        if (obj == null) return;
        
        Selectable s = obj.GetComponent<Selectable>();
        if (s != null) s.isSelected = false;
        obj = defaultSelection;
    }

    void UpdatePanel()
    {
        uiManager.ReplacePanel(selectedObject.GetComponent<Selectable>().uiPanel);
    }
}
