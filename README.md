# CoralLabeler

CoralRelabeler is a simple tool made to create or improve existing ground truths of coral reef image.
The tool was developed with speedy annotation in mind.

# How does it work?
The software uses Local Binary Patterns (LBP) to automatically extract a homogenously textured patch, with a gaussian dropout weight. The patch center is controlled by clicking, and the gaussian dropout sigma with mouse wheel (to grow and shrink the selection).

![GitHub Logo](/screenshot.jpg)

# Citation

Used within the context of my research project. If this tool was useful to you, please cite the following:

`Blanchet, Jean-Nicola. Automated texture-based recognition of corals in natural scene images. Diss. École de technologie supérieure, 2016.`

# License

this project is licensed under the MIT license
