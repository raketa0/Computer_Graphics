#pragma once

class LogicJumping
{
public:
    LogicJumping(float x, float baseY, float phaseOffset);
    void Update(float deltaTime);
    void StartJump(float velocity);
    bool IsJumping() const;
    float GetX() const;
    float GetY() const;

private:
    float m_jumpStartTime;
    float m_baseY;
    float m_x;
    float m_y;
    float m_initialVelocityY;
    float m_gravity;
    float m_time;
    float m_phaseOffset;
    bool m_isJumping;
};