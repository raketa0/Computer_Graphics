#include "FenceSection.h"
#include "RectangleShape.h"
#include "TriangleShape.h"


FenceSection::FenceSection(const sf::Vector2f& sizeRectangle,
	const sf::Vector2f& positionRectangle,
	const sf::Color& color)
{
	m_parts.push_back(std::make_unique<RectangleShape>(sizeRectangle , positionRectangle, color));
	m_parts.push_back(std::make_unique<TriangleShape>(
		sf::Vector2f(positionRectangle.x, positionRectangle.y),
		sf::Vector2f(positionRectangle.x + sizeRectangle.x / 2.0f,
			positionRectangle.y - sizeRectangle.y / 2.0f),
		sf::Vector2f(positionRectangle.x + sizeRectangle.x, positionRectangle.y),
		color));
}

void FenceSection::Draw(sf::RenderWindow& window)
{
	for (const auto& part : m_parts)
	{
		part->Draw(window);
	}
}

void FenceSection::Move(sf::Vector2f& delta)
{
	for (const auto& part : m_parts)
	{
		part->Move(delta);
	}
}

bool FenceSection::IsClick(sf::Vector2f& point)
{
	for (const auto& part : m_parts)
	{
		if (part->IsClick(point))
		{
			return true;
		}
	}
	return false;
}

