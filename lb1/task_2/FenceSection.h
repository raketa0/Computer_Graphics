#pragma once
#include "IShape.h"
#include <memory>
#include <vector>


class FenceSection : public IShape
{
public:
	FenceSection(const sf::Vector2f& sizeRectangle,
		const sf::Vector2f& positionRectangle,
		const sf::Color& color);
	void Draw(sf::RenderWindow& window) override;
	void Move(sf::Vector2f& delta) override;
	bool IsClick(sf::Vector2f& point) override;

private:
	std::vector<std::unique_ptr<IShape>> m_parts;
};