#pragma once
#include <SFML/Graphics.hpp>

class ConstructionCircle
{
public:
	ConstructionCircle(const int radius,
		const sf::Vector2i& center);

	void Draw(sf::RenderWindow& window);

private:
	int distanceError = 0;
	int m_r;
	sf::Vector2i m_center;
	void DrawPoint(sf::RenderWindow& window, sf::Vector2i pos);
};

