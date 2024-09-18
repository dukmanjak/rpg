using Godot;
using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

public abstract partial class Character : CharacterBody3D
{
	[Export] private StatResource[] stats;

	[ExportGroup("Required Nodes")]
	[Export] public AnimationPlayer AnimPlayerNode {get; private set; } 
	[Export] public Sprite3D SpriteNode {get; private set; } 
	[Export] public StateMachine StateMachineNode {get; private set; } 
	[Export] public Area3D Hurtboxnode {get; private set;}
	[Export] public Area3D HitboxNode {get; private set;}

	[ExportGroup("AI Nodes")]
	[Export] public Path3D PathNode { get; private set; }
	[Export] public NavigationAgent3D AgentNode  { get; private set; }
	[Export] public Area3D ChaseAreaNode {get; private set; }
	[Export] public Area3D AttackAreaNode {get; private set; }
	public Vector2 direction = new();

	public override void _Ready()
	{
		base._Ready();
		Hurtboxnode.AreaEntered += HandleHurtboxEntered;
	}

    private void HandleHurtboxEntered(Area3D area)
    {
        StatResource health = GetStatResource(Stat.Health);

		Character player = area.GetOwner<Character>();

		health.StatValue -= player.GetStatResource(Stat.Strength).StatValue;

		GD.Print(health.StatValue);
    }


    public void Flip()
	{
		bool isNotMovingHorizontally = Velocity.X == 0; 

		if (isNotMovingHorizontally) { return; }

		bool isMovingLeft = Velocity.X < 0;
		SpriteNode.FlipH = isMovingLeft;
	}

    public StatResource GetStatResource(Stat stat)
    {
		return stats.Where((element) => element.StatType == stat)
		.FirstOrDefault();
    }
}
