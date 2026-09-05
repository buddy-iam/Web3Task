# 2D Slingshot Physics Game

A 2D physics puzzle game developed in Unity where players launch a projectile using drag-and-release slingshot mechanics across environmental obstacles into a bucket sensor[cite: 1, 3]. Built and exported as an **ARM64 APK** for mobile devices[cite: 3].

---

## Controls Explanation

The game utilizes Unity's Legacy Input system (`Input.GetMouseButton` / `Input.mousePosition`), allowing uniform input handling across both desktop and mobile touchscreens[cite: 3]:

* **Desktop Controls:**
  * **Left Mouse Button (Click & Drag):** Click anywhere on the screen and pull back to aim the launch vector[cite: 3]. A dynamic trajectory line projects the arc in real time[cite: 3].
  * **Left Mouse Button (Release):** Launches the ball with an impulse force proportional to the drag distance[cite: 3]. Drag distance is clamped to a maximum threshold (`maxDragLength = 3.0f`) to cap launch velocity[cite: 3].
  * **Key 'R':** Instantly reloads the current scene via `SceneManager.LoadScene()`[cite: 4].
* **Mobile Controls:**
  * **Single-Finger Drag & Release:** Touch and drag to aim; release to launch the ball[cite: 3].
  * **Landscape Mode Prerequisite:** The game does not currently auto-orient the display into landscape mode; players must manually rotate or lock their device into landscape orientation to keep the playfield and targets properly visible.
  * **On-Screen Buttons:** Tap the **Retry** button during an attempt to restart, or the **Restart Level** button when the Victory screen appears[cite: 1, 4].

---

## Implementation Approach

* **Trajectory Arc Visualization (`ReadInput.cs`):**
  * Employs kinematic equations ($P(t) = P_0 + V_0t + \frac{1}{2}gt^2$) sampled over 15 discrete prediction points[cite: 3].
  * Renders the trajectory path using a `LineRenderer` component while dragging[cite: 3].
  * Applies an impulse force through `Rigidbody2D.AddForce` and immediately locks input (`hasLaunched = true`) to enforce a single-attempt launch rule[cite: 3].
* **Scene Reset Architecture (`LevelManager.cs`):**
  * Rather than relying on a complex singleton instance pattern (`LevelManager.Instance`), the game uses a direct scene reload via `SceneManager.LoadScene(SceneManager.GetActiveScene().name)`[cite: 4].
  * Reloading the active scene automatically resets all GameObjects, physical velocity vectors, and inspector references to their baseline state without requiring runtime respawn logic[cite: 4].
* **Direct UI Management (`CollideDecision.cs`):**
  * The ball handles its own game-state UI transitions directly in the scene[cite: 1].
  * Activates the `VictoryPanel` and hides the active `Retry` button upon landing a successful shot[cite: 1].
* **Testing & Ball Lifecycle (`Lifecycle.cs`):**
  * Implemented an in-engine **Retry** button from the beginning to accelerate iterative playtesting without having to stop and re-enter Play Mode repeatedly.
  * Included a 12-second stationary fail timer (`timer >= 12f`) intended to detect when the ball came to a permanent stop post-collision[cite: 2]. Because automated static-ball detection was not fully implemented, failure handling relies on the player using the on-screen retry button[cite: 1, 2].

---

## How Surface Interaction Logic Was Handled

The collision and win/loss resolution is handled directly inside `CollideDecision.cs` using simple name-matching checks without custom boolean flag systems[cite: 1]:

* **Physical Slope Collision (`OnCollisionEnter2D`):**
  * Detects physical contact with the environment via `collision.gameObject.name`[cite: 1].
  * If the ball collides with `"Slope"`, it logs a successful slope hit[cite: 1].
  * If the ball physically collides with any other surface, it immediately routes to `TriggerFail()` (logging `"Try Again!"`)[cite: 1].
* **Bucket Detection (`OnTriggerEnter2D`):**
  * An empty child GameObject within the bucket features a 2D Collider configured with **Is Trigger** checked.
  * When the ball enters this trigger, it checks `collision.gameObject.name == "Sensor"`[cite: 1].
  * Entering `"Sensor"` triggers `TriggerSuccess()`, activating the `VictoryPanel` and disabling the `Retry` button[cite: 1].
  * Any other trigger collision routes to `TriggerFail()`[cite: 1].

---

## Challenges Faced

* **Predictable Projectile Arc Math:**
  * Calculating realistic ballistic motion and dynamically aligning the `LineRenderer` to match the exact impulse velocity of `Rigidbody2D` required extensive research across Unity documentation, online tutorials (YouTube), and AI assistance to properly balance gravity scale, force factor, and time-step sampling[cite: 3].
* **Slope Construction with EdgeCollider2D:**
  * Creating a visible, accurately angled slope was difficult. Researching Unity docs, Reddit, and Quora led to using an `EdgeCollider2D` paired with a line material with customizable edge points and adjustable width so the slope stayed physically solid and clearly visible to the player.
* **Scene-to-Prefab Reference "Type Mismatch":**
  * Attempting to wire UI panels and scene-specific objects into the ball prefab inside the Project folder caused a Unity "Type Mismatch" / blocked assignment.
  * *Resolution:* Abandoned the dynamic ball respawn prefab pipeline; transitioned to directly configuring the ball inside the scene hierarchy and using full scene reloads to recover all initial references cleanly.
* **Screen Dimmer / Fade-Off Visibility:**
  * Attempted to implement a darkened backdrop fade to isolate and focus on the Victory prompt. However, ambient world geometry and 2D sprites rendered through the overlay unaffected due to canvas layering and alpha settings, leading to the removal of the backdrop dimmer in favor of a clean, standalone Victory panel[cite: 1].
* **Mobile Build UI & Input Issues:**
  * The **Retry** button functioned in the Editor but disappeared in the mobile APK build. The Canvas was positioned outside the mobile camera's viewport; adjusting Canvas placement resolved visibility.
  * The UI button initially refused to register touches because the `EventSystem` was assigned the new `Input System UI Input Module`. Replacing it with the legacy **Standalone Input Module** restored tap and click detection.

---

## Improvements if Given More Time

* **Automatic Orientation Lock:** Force the Android player settings (`Screen Orientation`) to strictly enforce **Landscape Left / Landscape Right** so the viewport never renders vertically on launch.
* **Reliable Stationary Ball Detection:** Complete the 12-second countdown logic in `Lifecycle.cs` by sampling `Rigidbody2D.linearVelocity` and automatically popping a dedicated "Defeat / Try Again" modal whenever the ball comes to a complete rest outside the bucket[cite: 2].
* **Proper Full-Screen Backdrop Curtain:** Implement a functional UI masking/curtain layer that completely eclipses the 2D background world when `TriggerSuccess()` executes[cite: 1].
* **True Ball Respawning:** Build a clean spawning manager capable of instantiating the ball prefab at its initial transform position without requiring a full scene reload.