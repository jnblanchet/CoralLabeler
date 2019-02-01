using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIMS_Relabeler.CoralReader.Model;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Threading.Tasks;

namespace AIMS_Relabeler.CoralReader
{
    // Reads images and also caches previous and following images asynchronously to enable faster navigation.
    class CoralProxyImageReader
    {
        private readonly string _rootFolder;
        private readonly ICollection<CoralPatch> _patches;

        private int _lastLoadedIndex = -1;
        private Image<Rgb, byte> _lastLoadedImage;

        private int _nextLoadedIndex = -1;
        private Image<Rgb, byte> _nextImage;

        public readonly int Count;

        public CoralProxyImageReader(ICollection<CoralPatch> patches, string root)
        {
            _rootFolder = root;
            _patches = patches;
            Count = patches.Count;
        }


        public Image<Rgb, byte> GetImage(int coralIndex)
        {
            // if it's the same image, return last one.
            if (_lastLoadedIndex >= 0 && _patches.ElementAt(coralIndex).FilePath.Equals(_patches.ElementAt(_lastLoadedIndex).FilePath))
                return _lastLoadedImage;

            // if it's the following one, switch to it
            else if (_nextLoadedIndex > -1 && _patches.ElementAt(coralIndex).FilePath.Equals(_patches.ElementAt(_lastLoadedIndex).FilePath))
                _lastLoadedImage = _nextImage;

            //its a user-selected specific iamge
            else
                _lastLoadedImage = new Image<Rgb, byte>(_rootFolder + "\\" + _patches.ElementAt(coralIndex).FilePath);

            // cache next image
            new Task(LoadNextImage).Start();

            _lastLoadedIndex = coralIndex;
            return _lastLoadedImage;
        }

        private void LoadNextImage()
        {
            var nextIndex = _lastLoadedIndex + 1;
            if (nextIndex < Count)
            {
                _nextLoadedIndex = nextIndex;
                if (!_patches.ElementAt(_lastLoadedIndex).FilePath.Equals(_patches.ElementAt(_nextLoadedIndex).FilePath))
                    _nextImage = new Image<Rgb, byte>(_rootFolder + "\\" + _patches.ElementAt(_nextLoadedIndex).FilePath);
            }
        }
    }
}
