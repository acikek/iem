# LPS Calculation

> **LPS**: LEXP/s

Each Emotion has unlockable Upgrades. These can Boost (or modify the result of LEXP calculation) any other valid Emotion, including itself, but nothing else. Additionally, emotions can boost based on Level.

To find the total boost for an emotion, one must first calculate all the **unlocked** upgrades' boosts (from all unlocked emotions) and check if they include the target emotion. The total boost is all of those boost values multiplied together.

The final LPS calculation for each emotion is:

`(baseLPS + 0.1 * tier * level) * boost`

Add these values together, for all emotions, to find the total LPS.

## When to calculate LPS

- When unlocking an emotion for the first time, only its LPS should be calculated.
- Total LPS should be calculated whenever a player levels up an emotion (as leveling can affect boosting) and whenever new upgrades are unlocked.