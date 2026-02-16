#pragma once
#include "IShape.h"
#include <memory>
#include <vector>


class House : public IShape
{

public:
	House(const sf::Vector2f& position);
	void Draw(sf::RenderWindow& window) override;
	void Move(sf::Vector2f& delta) override;
	bool IsClick(sf::Vector2f& point) override;

private:
	std::vector<std::unique_ptr<IShape>> m_parts;
	sf::Vector2f m_position;
	void CreateWall();
	void CreateRoof();
	void CreateDoor();
	void CreateWindow();
	void CreateFence();
};