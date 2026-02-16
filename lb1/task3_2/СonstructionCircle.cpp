#include "ÑonstructionCircle.h"

ConstructionCircle::ConstructionCircle(const int radius, 
	const sf::Vector2i& center)
	: m_r(radius), m_center(center){}

void ConstructionCircle::Draw(sf::RenderWindow& window)
{
	int x = 0;
	int y = m_r;
	distanceError = 3 - 2 * m_r;

	while (x <= y)
	{
		DrawPoint(window, { m_center.x + x, m_center.y + y });
		DrawPoint(window, { m_center.x - x, m_center.y + y });
		DrawPoint(window, { m_center.x + x, m_center.y - y });
		DrawPoint(window, { m_center.x - x, m_center.y - y });
		DrawPoint(window, { m_center.x + y, m_center.y + x });
		DrawPoint(window, { m_center.x - y, m_center.y + x });
		DrawPoint(window, { m_center.x + y, m_center.y - x });
		DrawPoint(window, { m_center.x - y, m_center.y - x });

		if (distanceError <= 0)
		{
			distanceError = distanceError + 4 * x + 6;
		}
		else
		{
			distanceError = distanceError + 4 * (x - y) + 10;
			y = y - 1;
		}

		x = x + 1;
	}
}

void ConstructionCircle::DrawPoint(sf::RenderWindow& window, sf::Vector2i pos)
{
	sf::Vertex point;
	point.position = sf::Vector2f(static_cast<float>(pos.x), static_cast<float>(pos.y));
	point.color = sf::Color::Black;
	window.draw(&point, 1, sf::PrimitiveType::Points);
}
