using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

[Serializable]
public class ProbeComponent : MonoBehaviour
{
    public string partName;
    public string description;
    private Sprite sprite;
    public int id;
    private int quantity;
    public readonly int max_quantity = 10;


    public ProbeComponent(string partName, string description, int id)
    {
        this.partName = partName;
        this.description = description;
        //this.sprite = sprite;
        this.id = id;
        this.quantity = 0;
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

    public ProbeComponent Clone()
    {
        
        return new ProbeComponent(partName, description, id);
    }
}
