#pragma once
#include "IShape.h"

class RectangleShape : public IShape
{
public:
	RectangleShape(const sf::Vector2f& size, 
		const sf::Vector2f& position, 
		const sf::Color& color);
	void Draw(sf::RenderWindow& window) override;
	void Move(sf::Vector2f& delta) override;
	bool IsClick(sf::Vector2f& point) override;

private:
	sf::RectangleShape m_rectangle;
};