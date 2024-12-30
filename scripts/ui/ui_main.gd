extends Control

var greenTex = preload("res://assets/ui/healthbar/bar_square_gloss_small_green.png")
var yellowTex = preload("res://assets/ui/healthbar/bar_square_gloss_small_yellow.png")
var redTex = preload("res://assets/ui/healthbar/bar_square_gloss_small_red.png")

@onready var healthbar = $Margin/HBox/HealthBar
@onready var proximity_label = $Margin/TextureRect/Label

func update_healthbar(value: int) -> void:
	healthbar.texture_progress = greenTex
	if value <= 100:
		healthbar.texture_progress = yellowTex
	if value <= 50:
		healthbar.texture_progress = redTex
		
	healthbar.value = value

func update_proximity_label(prox: int) -> void:
	print(prox)
	if prox == 1:
		proximity_label.text = "PROXIMITY: NEGATIVE"
	elif prox == 0:
		proximity_label.text = "PROXIMITY: POSITIVE"
