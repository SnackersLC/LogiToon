using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace ToontownLGP
{
    internal class ToontownLGPApp
    {
        public ToontownLGPApp()
        {
            ApplicationConfiguration.Initialize();

            _is_working = true;

            _logitech = new LogitechGP();
            _toontown = new Toontown();
            _window = new ToontownLGPWindow();
            _converter = new ImageConverter();

            if (!_logitech.Init())
            {
                Console.WriteLine("[ERROR] Logitech game panel SDK init failed.");
                //throw new Exception("Logitech game panel SDK init failed");
            }

            _show_global_population = false;

            Console.WriteLine("[INFO] Application inited.");
        }

        public void Run()
        {
            _toontown.Start();

            Thread thread = new Thread(() => this.LogicThread());
            thread.Start();

            Application.Run(_window);

            Console.WriteLine("[INFO] Window closed. Shutdown started.");

            _is_working = false;
            thread.Join();
            _toontown.Stop();

            Console.WriteLine("[INFO] Logic thread joined.");
        }

        public void LogicThread()
        {
            bool is_loading_image_displyed = false;
            bool is_first_hide = true;

            Bitmap[]? images_to_draw = null;
            Byte[][]? bytes_to_draw = null;
            int current_animation_index = 0;
            DateTime? current_animation_start_time = null;

            Console.WriteLine("[INFO] Toontown thread started");
            while (_is_working)
            {
                _logitech.Update();

                bool update_required = false;

                Toontown.ToontownState? update = _toontown.GetUpdate();

                if (update != null)
                {
                    _current_toontown_state = update.Value;
                    update_required = true;
                }

                ToontownLGPWindow.CONNECTION_STATUS local_connection_status = _current_toontown_state.local_connection_issue ? ToontownLGPWindow.CONNECTION_STATUS.CONNECTION_ISSUE
                    : _current_toontown_state.local_connected ? ToontownLGPWindow.CONNECTION_STATUS.CONNECTED : ToontownLGPWindow.CONNECTION_STATUS.CONNECTING;
                ToontownLGPWindow.CONNECTION_STATUS population_connection_status = _current_toontown_state.population_connection_issue ? ToontownLGPWindow.CONNECTION_STATUS.CONNECTION_ISSUE
                    : _current_toontown_state.population_connected ? ToontownLGPWindow.CONNECTION_STATUS.CONNECTED : ToontownLGPWindow.CONNECTION_STATUS.CONNECTING;
                ToontownLGPWindow.CONNECTION_STATUS silly_connection_status = _current_toontown_state.silly_connection_issue ? ToontownLGPWindow.CONNECTION_STATUS.CONNECTION_ISSUE
                    : _current_toontown_state.silly_connected ? ToontownLGPWindow.CONNECTION_STATUS.CONNECTED : ToontownLGPWindow.CONNECTION_STATUS.CONNECTING;
                _window.SetToontownLocalConnectionStatus(local_connection_status);
                _window.SetToontownPopulationConnectionStatus(population_connection_status);
                _window.SetToontownSillyConnectionStatus(silly_connection_status);

                bool lcd_connected = _logitech.IsConnected();
                _window.SetLogitechGPConnectionStatus(lcd_connected ? ToontownLGPWindow.CONNECTION_STATUS.CONNECTED : ToontownLGPWindow.CONNECTION_STATUS.NOT_CONNECTED);

                if ((!_current_toontown_state.local_connected || !_current_toontown_state.population_connected || !_current_toontown_state.silly_connected) && !is_loading_image_displyed)
                {
                    is_loading_image_displyed = true;
                    _logitech.SetBackground(GetBytes(Resources.loading_image));
                    _window.SetCurrentPicture(Resources.loading_image);
                    Thread.Sleep(200);
                    continue;
                }
                else if (!_current_toontown_state.local_connected || !_current_toontown_state.population_connected || !_current_toontown_state.silly_connected)
                {
                    continue;
                }
                is_loading_image_displyed = false;

                _window.SetHideInsteadClose(_current_toontown_state.local_connected && _current_toontown_state.population_connected && _current_toontown_state.silly_connected && lcd_connected);
                //_window.SetHideInsteadClose(_current_toontown_state.local_connected && _current_toontown_state.population_connected && _current_toontown_state.silly_connected && true);

                if (_current_toontown_state.local_connected && _current_toontown_state.population_connected && _current_toontown_state.silly_connected && lcd_connected && is_first_hide)
                //if (_current_toontown_state.local_connected && _current_toontown_state.population_connected && _current_toontown_state.silly_connected && true && is_first_hide)
                {
                    _window.HideWindow();
                    is_first_hide = false;
                }

                if (_logitech.IsActionButtonsPressed())
                {
                    _show_global_population = !_show_global_population;
                    update_required = true;
                }

                if (update_required)
                {
                    images_to_draw = GenerateBitmaps(
                        _current_toontown_state.toon_name, 
                        _current_toontown_state.toon_species,
                        _current_toontown_state.toon_current_laf, 
                        _current_toontown_state.toon_location_district, 
                        _current_toontown_state.toon_popolation, 
                        _current_toontown_state.total_popolation, 
                        _show_global_population, 
                        _current_toontown_state.silly_meter_state, 
                        _current_toontown_state.silly_meter_hp);

                    bytes_to_draw = GetBytes(images_to_draw);
                    current_animation_index = 0;

                    if (images_to_draw.Length > 1)
                    {
                        current_animation_start_time = DateTime.Now;
                    }
                    _logitech.SetBackground(bytes_to_draw[0]);
                    _window.SetCurrentPicture(images_to_draw[0]);
                }

                if(images_to_draw.Length > 1 && current_animation_start_time is not null)
                {
                    DateTime start_time = current_animation_start_time ?? DateTime.Now;
                    double seconds = (DateTime.Now - start_time).TotalSeconds;

                    int new_index = (int)Math.Floor(seconds) % images_to_draw.Length;

                    if(new_index != current_animation_index)
                    {
                        current_animation_index = new_index;
                        _logitech.SetBackground(bytes_to_draw[current_animation_index]);
                        _window.SetCurrentPicture(images_to_draw[current_animation_index]);
                    }
                }
            }

            Console.WriteLine("[INFO] Logic thread stopped.");
        }

        private Byte[] GetBytes(Bitmap bitmap)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData result_bitmap_data = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            Int32 bytes = result_bitmap_data.Stride * bitmap.Height;

            Byte[] result_bitma_rgba_values = new byte[bytes];
            Marshal.Copy(result_bitmap_data.Scan0, result_bitma_rgba_values, 0, bytes);
            bitmap.UnlockBits(result_bitmap_data);

            int result_r_size = bitmap.Width * bitmap.Height;
            Byte[] result_r_values = new byte[result_r_size];
            for (int i = 0; i < result_r_size; ++i)
            {
                if (result_bitma_rgba_values[i * 4] > 0) result_r_values[i] = 255;
                if (result_bitma_rgba_values[i * 4 + 1] > 0) result_r_values[i] = 255;
                if (result_bitma_rgba_values[i * 4 + 2] > 0) result_r_values[i] = 255;
                if (result_bitma_rgba_values[i * 4 + 3] > 0) result_r_values[i] = 255;
            }
            return result_r_values;
        }

        private Byte[][] GetBytes(Bitmap[] bitmaps)
        {
            Byte[][] result_bytes = Array.Empty<Byte[]>();
            foreach (Bitmap bitmap in bitmaps)
            {
                result_bytes = result_bytes.Append(GetBytes(bitmap)).ToArray();
            }

            return result_bytes;
        }

        private Bitmap GetToonIcon(String toon_species, int toon_current_laf)
        {
            switch (toon_species)
            {
                case "bear":
                    {
                        return toon_current_laf >= 1 ? Resources.bear : Resources.bear_sad;
                    }
                case "cat":
                    {
                        return toon_current_laf >= 1 ? Resources.cat : Resources.cat_sad;
                    }
                case "crocodile":
                    {
                        return toon_current_laf >= 1 ? Resources.crocodile : Resources.crocodile_sad;
                    }
                case "deer":
                    {
                        return toon_current_laf >= 1 ? Resources.deer : Resources.deer_sad;
                    }
                case "dog":
                    {
                        return toon_current_laf >= 1 ? Resources.dog : Resources.dog_sad;
                    }
                case "duck":
                    {
                        return toon_current_laf >= 1 ? Resources.duck : Resources.duck_sad;
                    }
                case "horse":
                    {
                        return toon_current_laf >= 1 ? Resources.horse : Resources.horse_sad;
                    }
                case "monkey":
                    {
                        return toon_current_laf >= 1 ? Resources.monkey : Resources.monkey_sad;
                    }
                case "mouse":
                    {
                        return toon_current_laf >= 1 ? Resources.mouse : Resources.mouse_sad;
                    }
                case "pig":
                    {
                        return toon_current_laf >= 1 ? Resources.pig : Resources.pig_sad;
                    }
                case "rabbit":
                    {
                        return toon_current_laf >= 1 ? Resources.rabbit : Resources.rabbit_sad;
                    }
            }
            return toon_current_laf > 1 ? Resources.rabbit : Resources.rabbit_sad;
        }

        private Bitmap[] GetSillyIcons(String silly_state, int silly_hp)
        {
            if (silly_state == "Active")
            {
                if (silly_hp >= 5000000) { return [Resources.meterfull]; }
                else if (silly_hp >= 3000001) { return [Resources.meter3_1, Resources.meter3_2]; }
                else if (silly_hp >= 1500001) { return [Resources.meter2_1, Resources.meter2_2]; }
                else if (silly_hp >= 1) { return [Resources.meter1_1, Resources.meter1_2]; }
            }
            else if (silly_state == "Reward") { return [Resources.meterfull]; }
            else if (silly_state == "Inactive") { return [Resources.meterrecharge]; }

            return [Resources.meterrecharge];
        }

        private String GetSillyHpText(String state, float silly_hp)
        {
            if (state == "Inactive") { return "Recharging"; }

            if (silly_hp > 999)
            {
                if (silly_hp > 1000000) { return String.Format("{0:0.0}M", (Math.Floor(silly_hp / 100000) / 10).ToString()); }
                return String.Format("{0:0.0}K", (Math.Floor(silly_hp / 100) / 10).ToString());
            }

            return silly_hp.ToString();
        }

        Bitmap[] GenerateBitmaps(String? name, String? species, int? laf, String? district, int? population, int? total_population, bool show_global_population, String? silly_state, int? silly_hp)
        {
            Bitmap[] result_bitmaps = Array.Empty<Bitmap>();

            int image_resolution = Int32.Parse(Resources.image_default_resolution);

            StringFormat string_format = new StringFormat();
            string_format.Alignment = StringAlignment.Center;
            Font font = new Font("11pxbus", 10);

            Bitmap toon_icon = GetToonIcon(species ?? "unknown", laf ?? 0);
            Bitmap[] silly_icons = GetSillyIcons(silly_state ?? "unknown", silly_hp ?? 0);
            Bitmap district_image = Resources.district_logo;

            toon_icon.SetResolution(image_resolution, image_resolution);
            district_image.SetResolution(image_resolution, image_resolution);

            foreach (Bitmap silly_icon in silly_icons)
            {
                Bitmap bitmap_to_draw = new Bitmap(160, 43);
                bitmap_to_draw.SetResolution(image_resolution, image_resolution);

                silly_icon.SetResolution(bitmap_to_draw.HorizontalResolution, bitmap_to_draw.VerticalResolution);

                Graphics g = Graphics.FromImage(bitmap_to_draw);
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;

                g.DrawImage(toon_icon, new Point(0, 0));
                g.DrawImage(district_image, new Point(_show_global_population ? -8 : 0, 0));
                g.DrawImage(silly_icon, new Point(0, 0));

                SizeF name_text_size = g.MeasureString(name, font);
                RectangleF name_text_rectangle = new RectangleF(0, 43 - name_text_size.Height, 160, name_text_size.Height);

                SizeF laff_text_size = g.MeasureString(laf.ToString(), font);
                RectangleF laff_text_rectangle = new RectangleF(0, 25 - laff_text_size.Height, 160, laff_text_size.Height);

                String title_text = show_global_population ? Resources.image_global_population_title : district ?? "unknown";
                SizeF title_text_size = g.MeasureString(title_text, font);
                float title_text_x = show_global_population ? 121 - title_text_size.Width / 2 : 127 - title_text_size.Width / 2;
                RectangleF title_text_rectangle = new RectangleF(title_text_x, 36 - title_text_size.Height, title_text_size.Width, title_text_size.Height);

                String count_text = (show_global_population ? total_population ?? 0 : population ?? 0).ToString();
                SizeF count_text_size = g.MeasureString(count_text, font);
                float count_text_x = show_global_population ? 130 : 140;
                RectangleF count_text_rectangle = new RectangleF(count_text_x, 21 - count_text_size.Height, count_text_size.Width, count_text_size.Height);

                String silly_hp_text = GetSillyHpText(silly_state ?? "", silly_hp ?? 0);
                SizeF silly_hp_text_size = g.MeasureString(silly_hp_text, font);
                RectangleF silly_hp_text_rectangle = new RectangleF(32 - silly_hp_text_size.Width / 2, 0, silly_hp_text_size.Width, silly_hp_text_size.Height);

                g.DrawString(name, font, Brushes.Black, name_text_rectangle, string_format);
                g.DrawString(title_text, font, Brushes.Black, title_text_rectangle, string_format);
                g.DrawString(count_text, font, Brushes.Black, count_text_rectangle, string_format);
                g.DrawString(silly_hp_text, font, Brushes.Black, silly_hp_text_rectangle, string_format);

                if (laf >= 1) { g.DrawString(laf.ToString(), font, Brushes.Black, laff_text_rectangle, string_format); }

                g.Flush();

                result_bitmaps = result_bitmaps.Append(bitmap_to_draw).ToArray();
            }

            return result_bitmaps;
        }

        private bool _is_working;

        private LogitechGP _logitech;

        private Toontown _toontown;

        private ToontownLGPWindow _window;

        private ImageConverter _converter;

        private Toontown.ToontownState _current_toontown_state;
        private bool _show_global_population;
    }
}
