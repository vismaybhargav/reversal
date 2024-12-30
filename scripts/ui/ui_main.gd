extends Control

var greenTex = preload("res://assets/ui/healthbar/bar_square_gloss_small_green.png")
var yellowTex = preload("res://assets/ui/healthbar/bar_square_gloss_small_yellow.png")
var redTex = preload("res://assets/ui/healthbar/bar_square_gloss_small_red.png")

@onready var healthbar = $HealthBar
@onready var polarity = $PolarityRect/PolarityLabel

func update_healthbar(value: int) -> void:
	healthbar.texture_progress = greenTex
	if value <= 100:
		healthbar.texture_progress = yellowTex
	if value <= 50:
		healthbar.texture_progress = redTex
		
	healthbar.value = value

func update_proximity_label(prox: int) -> void:
	if prox == 0:
		polarity.text = "POSITIVE"
	elif prox == 1:
		polarity.text = "NEGATIVE"
