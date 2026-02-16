#include "CircleShape.h"

CircleShape::CircleShape(const float radius, 
	const sf::Vector2f& center, 
	const sf::Color& color)
{
	m_circle.setRadius(radius);
	m_circle.setPosition(center);
	m_circle.setFillColor(color);
}

void CircleShape::Draw(sf::RenderWindow& window)
{
	window.draw(m_circle);
}

void CircleShape::Move(sf::Vector2f& delta)
{
	m_circle.move(delta);
}

bool CircleShape::IsClick(sf::Vector2f& point)
{
	sf::Vector2f center = m_circle.getPosition();
    float radius = m_circle.getRadius();

    float dx = point.x - center.x;
    float dy = point.y - center.y;

    return (dx * dx + dy * dy) <= (radius * radius);
}