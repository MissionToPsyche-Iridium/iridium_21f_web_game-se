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
    private readonly int max_quantity = 10;


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
        this.quantity = newQuantity;
    }

    public void incrementQuantity() {
        this.quantity++;
    }

    public void decrementQuantity() {
        this.quantity--;
    }

    public ProbeComponent Clone()
    {
        //TODO creat new id (get instance id?)
        return new ProbeComponent(name, description, sprite);
    }
}
