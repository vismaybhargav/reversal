extends Control

var greenTex = preload("res://assets/ui/healthbar/bar_square_gloss_small_green.png")
var yellowTex = preload("res://assets/ui/healthbar/bar_square_gloss_small_yellow.png")
var redTex = preload("res://assets/ui/healthbar/bar_square_gloss_small_red.png")

@onready var healthbar = $Margin/HBox/HealthBar

func update_healthbar(value: int) -> void:
	healthbar.texture_progress = greenTex
	if value <= 100:
		healthbar.texture_progress = yellowTex
	if value <= 50:
		healthbar.texture_progress = redTex
		
	healthbar.value = value
