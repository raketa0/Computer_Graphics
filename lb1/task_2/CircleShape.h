#pragma once

#include "IShape.h"

class CircleShape : public IShape
{
public:
	CircleShape(const float radius, 
		const sf::Vector2f& center, 
		const sf::Color& color);
	void Draw(sf::RenderWindow& window) override;
	void Move(sf::Vector2f& delta) override;
	bool IsClick(sf::Vector2f& point) override;

private:
	sf::CircleShape m_circle;

};
