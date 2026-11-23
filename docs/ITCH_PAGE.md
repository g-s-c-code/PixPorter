# PixPorter: itch.io page

Source of truth for the itch page. Paste the fields into https://itch.io/game/edit/<id>.

## Fields

| field | value |
|---|---|
| Title | PixPorter |
| Project URL | `pixporter` (https://g-s-c-code.itch.io/pixporter) |
| Short description | Drop in images or whole folders, pick a format, convert. Fast, parallel batch converting for 9 formats on Windows. |
| Classification | Tools |
| Kind of project | Downloadable |
| Release status | Released |
| Pricing | Paid, minimum $1.00, "allow people to pay more" ON |
| Uploads | `pixporter-windows.zip` (channel `windows`, tagged Windows) |
| Genre / category | (tools have no genre) |
| Tags (10) | image-converter, webp, png, jpg, batch, converter, utility, windows, photography, graphics |
| AI disclosure | Answer as the owner did for unit12 (code/text where applicable) |
| Community | Comments: owner's choice |
| Visibility | Draft until the owner says "go" |
| Cover | `docs/store/cover.png` (630x500) |
| Screenshots | `docs/store/screenshot-1.png` ... `screenshot-5.png` (in order) |
| Theme | BG `#0a0a0a`, BG2 `#161616`, text `#d4d4d4`, links `#7eb8f7`, buttons `#e8e8e8` |

## Install instructions (field)

```
1. Download pixporter-windows.zip and unzip it anywhere (e.g. Documents\PixPorter).
2. Run PixPorter.exe. No installer and no .NET install needed; everything is inside the folder.
3. Windows SmartScreen may say "Windows protected your PC" the first time, because the app is
   new and unsigned. Click "More info" -> "Run anyway".
4. Optional: right-click PixPorter.exe -> Send to -> Desktop (create shortcut), or use it via
   "Open with" on any image.
To uninstall, delete the folder.
```

## Description (HTML)

```html
<h2>Convert images in batches. Drop, pick, done.</h2>

<p><strong>PixPorter</strong> is a small, fast Windows desktop app for the image conversion you
keep doing by hand. Drop a pile of files, or a whole folder, onto the window, choose an output
format, press <em>Convert All</em>. Every image is converted in parallel, and a log tells you
exactly what happened to each one, what it weighed before and after, and where it was written.</p>

<p>No accounts, no uploads, no watermarks, no subscriptions. Everything happens on your own
machine, offline.</p>

<h2>Nine formats, any direction</h2>
<p><strong>PNG · JPG / JPEG · WebP · GIF · BMP · TIFF · TGA · QOI · PBM</strong></p>
<p>Convert from any of them to any other. Turn a folder of screenshots into compact WebP for a
website, get WebP downloads back into PNG so every program can open them, flatten TIFF scans into
JPG, or prepare TGA and QOI textures for game work.</p>

<h3>Smart defaults when you don't pick a format</h3>
<p>Leave the output format on <em>Default</em> and PixPorter chooses the sensible thing per file:</p>
<ul>
  <li><strong>WebP → PNG</strong>, so web images become universally editable.</li>
  <li><strong>PNG, JPG, GIF, TIFF, BMP, TGA, QOI, PBM → WebP</strong>, for much smaller files.</li>
</ul>

<h2>Built for batches</h2>
<ul>
  <li><strong>Drag and drop</strong> files or folders. A folder queues every supported image in it.</li>
  <li><strong>Browse Files</strong> picker, plus <strong>"Open with"</strong> and <strong>"Send to"</strong>
      from Explorer: images passed to PixPorter land straight in the queue.</li>
  <li><strong>Parallel conversion</strong>, limited to your processor count so the PC stays usable.</li>
  <li><strong>Cancel any time</strong>, even halfway through a big batch.</li>
  <li><strong>Per-file status</strong> in the queue, and a full <strong>log</strong> with input size,
      output size and the exact output path. If something fails, the log says why, instead of a
      vague "3 errors".</li>
</ul>

<h2>Quality you control, honestly</h2>
<ul>
  <li>A <strong>quality slider</strong> for JPG, WebP and PNG.</li>
  <li>For <strong>JPG and WebP</strong> it sets pixel fidelity: lower means smaller, lossier files.</li>
  <li>For <strong>PNG</strong> it only sets compression effort. PNG stays lossless at every setting;
      no pixel is ever thrown away.</li>
  <li>Formats that ignore quality <strong>say so</strong> instead of pretending to apply it.</li>
</ul>

<h2>Privacy: strip metadata</h2>
<p>One switch removes <strong>EXIF, IPTC, XMP and ICC</strong> data from the output: camera model,
GPS location, timestamps, editing history. Handy before you post photos online or send them to
someone.</p>

<h2>Your originals are safe</h2>
<p>PixPorter never overwrites a source image. Output goes next to the original by default, or into
any folder you choose. And in the two cases that would otherwise destroy a file, it writes a
<code>-converted</code> copy instead:</p>
<ul>
  <li>converting an image to the format it already is, and</li>
  <li>a batch holding both <code>photo.png</code> and <code>photo.webp</code>, where each would
      otherwise become the other's output.</li>
</ul>

<h2>Looks right on your desktop</h2>
<p>A clean, quiet interface with <strong>light and dark themes</strong> that follow your Windows
setting. A built-in <em>Information</em> tab lists every format, the default mappings and how
quality works, so you never need a manual.</p>

<h2>System requirements</h2>
<ul>
  <li>Windows 10 or 11, 64-bit</li>
  <li>No installer, no .NET runtime to install: unzip and run</li>
  <li>Works fully offline</li>
</ul>

<h2>Credits</h2>
<p>Image processing by <a href="https://sixlabors.com/products/imagesharp/">SixLabors.ImageSharp</a>,
licensed under the Apache License 2.0, © Six Labors. Its licence text is included in the download.</p>

<p>Questions or a format you would like added? Get in touch through my itch.io profile.</p>
```
