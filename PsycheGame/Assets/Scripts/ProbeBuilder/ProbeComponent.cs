using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

[Serializable]
public class ProbeComponent : MonoBehaviour
{
    public string partName;
    public string description;
    private Sprite sprite;
    public int id;
    private int quantity;
    private readonly int max_quantity = 10;
    public Vector3 position;
    private GameObject shape;


    public ProbeComponent(GameObject shape)
    {
        this.partName = shape.name;
        this.description = "no description available";
        //this.sprite = sprite;
        this.id = shape.GetInstanceID();
        this.quantity = 1;
        this.position = shape.transform.localPosition;
        this.shape = shape;
    }

    public string GetName()
    {
        return partName;
    }

    public string GetDescription()
    {
        return description;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public int getId() {
        return id;
    }

    public int getQuantity(){
        return quantity;
    }

    public void setQuantity(int newQuantity) {
        if(quantity <= max_quantity) {
            this.quantity = newQuantity;
        }
    }

    public void incrementQuantity() {
        if(quantity < max_quantity) {
            this.quantity++;
        }
    }

    public void decrementQuantity() {
        if(quantity > 0) {
            this.quantity--;
        }
    }

    void Update() {
        position = shape.transform.localPosition;
    }

    public ProbeComponent Clone()
    {
        return new ProbeComponent(shape);
    }
}
