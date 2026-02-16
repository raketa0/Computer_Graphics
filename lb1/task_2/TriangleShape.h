#pragma once

#include "IShape.h"

class TriangleShape : public IShape
{
public:
	TriangleShape(const sf::Vector2f& point1,
		const sf::Vector2f& point2,
		const sf::Vector2f& point3,
		const sf::Color& color);
	void Draw(sf::RenderWindow& window) override;
	void Move(sf::Vector2f& delta) override;
	bool IsClick(sf::Vector2f& point) override;

private:
	sf::ConvexShape m_triangle;

	float Area(sf::Vector2f& point1, sf::Vector2f& point2, sf::Vector2f& point3);
};
