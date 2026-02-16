#include "TriangleShape.h"

TriangleShape::TriangleShape(const sf::Vector2f& point1,
	const sf::Vector2f& point2,
	const sf::Vector2f& point3, 
	const sf::Color& color)
{
	m_triangle.setPointCount(3);
	m_triangle.setPoint(0, point1);
	m_triangle.setPoint(1, point2);
	m_triangle.setPoint(2, point3);
	m_triangle.setFillColor(color);
}

void TriangleShape::Draw(sf::RenderWindow& window)
{
	window.draw(m_triangle);
}

void TriangleShape::Move(sf::Vector2f& delta)
{
	m_triangle.move(delta);
}

bool TriangleShape::IsClick(sf::Vector2f& point)
{
	sf::Vector2f pos = m_triangle.getPosition();
	sf::Vector2f p1 = m_triangle.getPoint(0) + pos;
	sf::Vector2f p2 = m_triangle.getPoint(1) + pos;
	sf::Vector2f p3 = m_triangle.getPoint(2) + pos;

	float totalArea = Area(p1, p2, p3);
	float area1 = Area(point, p2, p3);
	float area2 = Area(p1, point, p3);
	float area3 = Area(p1, p2, point);

	return std::abs(totalArea - (area1 + area2 + area3)) < 1.0f;
}

float TriangleShape::Area(sf::Vector2f& point1, sf::Vector2f& point2, sf::Vector2f& point3)
{
	return std::abs((point1.x * (point2.y - point3.y) + 
		point2.x * (point3.y - point1.y) + 
		point3.x * (point1.y - point2.y)) / 2.0f);
}
