<h1>The Last Ticket</h1>

<h2>Description</h2>
<p>
  <strong>The Last Ticket</strong> is a first-person mystery investigation game built in Unity,
  where the player steps into the role of an investigator piecing together a murder that took place
  inside a late-night office. What begins with reading a simple news article about a missing person
  quickly unravels into a deeper, more personal crime — one hidden in plain sight across computers,
  printers, a microwave, and a concealed phone.
</p>
<p>
  The game is built around environmental deduction. There are no combat mechanics, no timers, and
  no hand-holding — only the office, its objects, and the clues they hold. Every interaction is
  deliberate. Every detail matters. The killer's identity and your fate depend entirely on what you
  find and what you choose to do with it.
</p>

<h2>Features</h2>
<ul>
  <li>First-person exploration and investigation gameplay</li>
  <li>Fully interactive office environment — computers, printer, microwave, hidden phone</li>
  <li>Computer login system with password discovery and wrong-password feedback</li>
  <li>Dynamic environmental storytelling — flickering lights, printer sounds, microwave triggers</li>
  <li>Rainy night atmosphere with rain-on-glass window shader and ambient audio layering</li>
  <li>Notification and popup system simulating a real desktop OS</li>
  <li>Progressive clue discovery tracked across 8 acts</li>
  <li>Central GameTracker system recording interactions, clues, choices, and play time</li>
  <li>Multiple endings based on what evidence you find and what you decide to do with it</li>
  <li>Secret ending for players who don't act at all</li>
</ul>

<h2>Game Flow</h2>
<ul>
  <li><strong>Act 1:</strong> Start at your own computer. Read a news article about a missing person. Receive a suspicious notification. Obtain the IP address of the victim's computer.</li>
  <li><strong>Act 2:</strong> Locate the victim's computer using the name tag and number. Log in. Read a congratulations email referencing a RAM item. Find a fake note. Note the error timestamp: <strong>02:57</strong>. Time of death: <strong>03:00</strong>.</li>
  <li><strong>Act 3:</strong> As you exit the victim's computer, the office lights flicker and the printer activates. Walk to the printer. Collect the printed note revealing the origin computer number: <strong>5.7</strong>.</li>
  <li><strong>Act 4:</strong> Find computer 5.7. The password hint is on the desk — dog name and date of birth (<strong>pjerkos04</strong>). Attempt login. Blue screen. Microwave sounds begin.</li>
  <li><strong>Act 5:</strong> The microwave door opens on its own, lights flicker. Find the hidden phone inside. Open the health app. Discover the time the victim's heart stopped.</li>
  <li><strong>Act 6:</strong> Access the IT department application (ItApp). Review fax logs and sign-in records. Cross-reference timestamps to confirm the exact time of death and identify who sent the final print request.</li>
  <li><strong>Act 7:</strong> Find blood on a computer. Interact with it. Discover the bloody RAM hidden inside.</li>
  <li><strong>Act 8 — Multiple Endings:</strong>
    <ul>
      <li>Turn in the suspect and lose the RAM</li>
      <li>Keep the RAM for yourself</li>
      <li>Sell the RAM on the black market</li>
      <li><em>Secret ending:</em> Don't press anything. Turn around slowly.</li>
    </ul>
  </li>
</ul>

<h2>Controls</h2>
<ul>
  <li><strong>WASD</strong> — Move</li>
  <li><strong>Mouse</strong> — Look around</li>
  <li><strong>E</strong> — Interact with objects</li>
  <li><strong>Escape</strong> — Close UI / exit interaction</li>
</ul>

<h2>Technologies Used</h2>
<ul>
  <li>Unity (Built-in Render Pipeline)</li>
  <li>C#</li>
  <li>TextMeshPro</li>
  <li>Unity Particle System</li>
  <li>Rain on Glass shader (toadstorm/RainyGlassShader)</li>
</ul>

<h2>Architecture Overview</h2>
<ul>
  <li><strong>GameTracker</strong> — singleton that tracks all interactions, clues discovered, story events completed, player choices, password attempts, and total play time</li>
  <li><strong>ComputerInteraction</strong> — handles login, wrong password feedback, and triggers Act 3 on successful login</li>
  <li><strong>MainMenu</strong> — manages the desktop OS simulation including popup slide animations and ItApp canvas</li>
  <li><strong>PlayerMovement / PlayerCam</strong> — first-person movement and camera with sensitivity controls</li>
  <li><strong>LampFlickerNoise</strong> — Perlin noise ambient flicker with a forceable triggered flicker mode for Act 3</li>
  <li><strong>PaperInteract</strong> — reusable interactable for in-world notes and documents</li>
  <li><strong>AmbientManager</strong> — layered audio system controlling AC hum, fluorescent buzz, and tension transitions</li>
</ul>

<h2>How to Run</h2>
<ol>
  <li>Clone the repository</li>
  <li>Open Unity Hub and add the project</li>
  <li>Open the main scene</li>
  <li>Press Play</li>
</ol>
<p>Recommended Unity version: <strong>2022.3 LTS or later</strong></p>

<h2>Contributors</h2>
<ul>
  <li>Name 1 – role / contribution</li>
  <li>Name 2 – role / contribution</li>
  <li>Name 3 – role / contribution</li>
</ul>

<h2>Notes</h2>
<ul>
  <li>The game is focused entirely on exploration and deduction — no combat, no timers.</li>
  <li>All endings are driven by choices made during Act 8. Earlier acts affect which options are available.</li>
  <li>The secret ending requires the player to do nothing when prompted.</li>
  <li>Planned improvements: Act 4–8 scripted interactions, save/load system, end screen with play stats, additional sound design, and UI polish.</li>
</ul>
