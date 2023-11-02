using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HeightmapTerrainStarter
{
    /// <summary>
    /// A camera controlled by WASD + Mouse
    /// </summary>
    public class FPSCamera : ICamera
    {
        // The angle of rotation about the Y-axis
        float horizontalAngle;

        // The angle of rotation about the X-axis
        float verticalAngle;

        Vector3 position;

        MouseState oldMouseState;

        public IHeightMap HeightMap { get; set; }

        Game game;

        /// <summary>
        /// Gets or sets how high above the heightmap the camera should be
        /// </summary>
        public float HeightOffset { get; set; } = 5;

        public float Sensitivity { get; set; } = 0.0018f;

        public float Speed { get; set; } = 0.5f;

        public Matrix View { get; protected set; }

        public Matrix Projection { get; protected set; }

        /// <summary>
        /// Updates the camera
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            var newMouseState = Mouse.GetState();

            // Get the direction the player is currently facing
            var facing = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(horizontalAngle));

            // Forward and backward movement
            if (keyboard.IsKeyDown(Keys.W)) position += facing * Speed;
            if (keyboard.IsKeyDown(Keys.S)) position -= facing * Speed;

            // Strifing movement
            if (keyboard.IsKeyDown(Keys.A)) position += Vector3.Cross(Vector3.Up, facing) * Speed;
            if (keyboard.IsKeyDown(Keys.D)) position -= Vector3.Cross(Vector3.Up, facing) * Speed;

            // Adjust horizontal angle
            horizontalAngle += Sensitivity * (oldMouseState.X - newMouseState.X);

            // Adjust vertical angle 
            verticalAngle += Sensitivity * (oldMouseState.Y - newMouseState.Y);

            // determine the direction the camera faces
            var direction = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationX(verticalAngle) * Matrix.CreateRotationY(horizontalAngle));

            // create the veiw matrix
            View = Matrix.CreateLookAt(position, position + direction, Vector3.Up);

            // Reset mouse state 
            Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            oldMouseState = Mouse.GetState();

            // Adjust the camera based off the height map
            if (HeightMap != null)
            {
                position.Y = HeightMap.GetHeightAt(position.X, position.Z) + HeightOffset;
            }
        }

        /// <summary>
        /// Constructs a new FPS Camera
        /// </summary>
        /// <param name="game">The game this camera belongs to</param>
        /// <param name="position">The player's initial position</param>
        public FPSCamera(Game game, Vector3 position)
        {
            this.game = game;
            this.position = position;

            this.horizontalAngle = 0;
            this.verticalAngle = 0;

            this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 1, 1000);
            Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            oldMouseState = Mouse.GetState();
        }
    }
}