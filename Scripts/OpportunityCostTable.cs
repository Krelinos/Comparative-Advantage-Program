using ComparativeAdvantage;
using Godot;
using System;

public class OpportunityCostTable : MarginContainer
{
    private Label LightPepperCost;
    private Label LightTomatoCost;
    private Label DarkPepperCost;
    private Label DarkTomatoCost;

    public override void _Ready()
    {
        var container = GetNode("CenterContainer/VBoxContainer");
        LightPepperCost = container.GetNode<Label>("HBoxContainer3/HBoxContainer/Label");
        LightTomatoCost = container.GetNode<Label>("HBoxContainer3/HBoxContainer2/Label");
        DarkPepperCost = container.GetNode<Label>("HBoxContainer4/HBoxContainer/Label");
        DarkTomatoCost = container.GetNode<Label>("HBoxContainer4/HBoxContainer2/Label");
    
        LightPepperCost.Text = Math.Round((double)GameService.Variables["light_pepper_opportunity_cost"], 3).ToString();
        LightTomatoCost.Text = Math.Round((double)GameService.Variables["light_tomato_opportunity_cost"], 3).ToString();
        DarkPepperCost.Text = Math.Round((double)GameService.Variables["dark_pepper_opportunity_cost"], 3).ToString();
        DarkTomatoCost.Text = Math.Round((double)GameService.Variables["dark_tomato_opportunity_cost"], 3).ToString();
    }
}
