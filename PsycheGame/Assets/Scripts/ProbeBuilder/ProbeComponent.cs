using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class ProbeComponent
{
    private string name;
    private string description;
    private Sprite sprite;
    private int id;
    private int quantity;
    public readonly int max_quantity = 10;


    public ProbeComponent(string name, string description, Sprite sprite)
    {
        this.name = name;
        this.description = description;
        this.sprite = sprite;
        this.id = sprite.GetInstanceID();
        this.quantity = 0;
    }

    public string GetName()
    {
        return name;
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

    // public ProbeComponent Clone()
    // {
    //     
    //     return new ProbeComponent(name, description, sprite);
    // }
}
