#include "RectangleShape.h"

RectangleShape::RectangleShape(const sf::Vector2f& size, 
	const sf::Vector2f& position,
	const sf::Color& color)
{
	m_rectangle.setSize(size);
	m_rectangle.setPosition(position);
	m_rectangle.setFillColor(color);
}

void RectangleShape::Draw(sf::RenderWindow& window)
{
	window.draw(m_rectangle);
}

void RectangleShape::Move(sf::Vector2f& delta)
{
	m_rectangle.move(delta);
}

bool RectangleShape::IsClick(sf::Vector2f& point)
{
	return m_rectangle.getGlobalBounds().contains(point);
}
