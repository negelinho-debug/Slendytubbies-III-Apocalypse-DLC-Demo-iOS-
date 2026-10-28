namespace MeshTK
{
	internal class ImageEdge
	{
		public ImagePoint v1;

		public ImagePoint v2;

		public ImageEdge(ImagePoint _v1, ImagePoint _v2)
		{
			v1 = _v1;
			v2 = _v2;
		}

		public void Reverse()
		{
			ImagePoint imagePoint = v1;
			v1 = v2;
			v2 = imagePoint;
		}
	}
}
