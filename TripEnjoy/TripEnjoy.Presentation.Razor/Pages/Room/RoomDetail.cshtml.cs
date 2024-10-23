using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages.Room
{
    public class RoomDetailModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty(SupportsGet = true)]
        public RoomDetail RoomDetail { get; set; }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<RoomImages> RoomImages { get; set; }
        [BindProperty(SupportsGet = true)]
        public IEnumerable<RoomDetail> RelatedRooms { get; set; }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<Rate> Rates { get; set; }  // Thêm thuộc tính để lưu trữ danh sách đánh giá

        [BindProperty(SupportsGet = true)]
        public IEnumerable<Comment> Comments { get; set; }
        public RoomDetailModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient("DefaultClient");
            var response = await client.GetAsync($"https://localhost:7126/api/Room/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                RoomDetail = JsonSerializer.Deserialize<RoomDetail>(data, option);

                // Goi API lay list image cua phong
                var imagesResponse = await client.GetAsync($"https://localhost:7126/api/RoomImage/images/{id}");
                if (imagesResponse.IsSuccessStatusCode)
                {
                    string imagesData = await imagesResponse.Content.ReadAsStringAsync();
                    RoomImages = JsonSerializer.Deserialize<List<RoomImages>>(imagesData, option);
                }

                // Gọi API để lấy danh sách phòng liên quan
                var relatedRoomsResponse = await client.GetAsync($"https://localhost:7126/api/Room/related/{RoomDetail.RoomTypeId}/{RoomDetail.HotelId}");
                if (relatedRoomsResponse.IsSuccessStatusCode)
                {
                    string relatedRoomsData = await relatedRoomsResponse.Content.ReadAsStringAsync();
                    RelatedRooms = JsonSerializer.Deserialize<List<RoomDetail>>(relatedRoomsData, option);
                    // Loại bỏ phòng hiện tại khỏi danh sách phòng liên quan
                    RelatedRooms = RelatedRooms.Where(room => room.RoomId != id).ToList();
                }

                // Lấy danh sách đánh giá
                var ratesResponse = await client.GetAsync($"https://localhost:7126/api/Rate/Room/{id}");
                if (ratesResponse.IsSuccessStatusCode)
                {
                    string ratesData = await ratesResponse.Content.ReadAsStringAsync();
                    Rates = JsonSerializer.Deserialize<List<Rate>>(ratesData, option);
                }

                var commentsResponse = await client.GetAsync($"https://localhost:7126/api/Comment/Room/{id}");
                if (commentsResponse.IsSuccessStatusCode)
                {
                    string commentsData = await commentsResponse.Content.ReadAsStringAsync();
                    Comments = JsonSerializer.Deserialize<List<Comment>>(commentsData, option);
                }


                return Page();
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(); 
            }
            
            return Page();
            
        }

        public async Task<IActionResult> OnPostReply(int id)
        {
            var accessToken = Request.Cookies["accessToken"];
            if (accessToken == null)
            {
                // Có thể trả về một lỗi hoặc điều hướng người dùng đăng nhập
                return Unauthorized();  // Sử dụng Unauthorized thay vì throw Exception
            }

            var replyRequest = new
            {
                CommentId = id,  // Truyền đúng ID của comment cần trả lời
                RoomId = RoomDetail.RoomId,  // Truyền RoomId hiện tại
                                             // Có thể thêm trường content nếu phản hồi cần nội dung
                Content = "" // Bạn có thể thay thế bằng dữ liệu từ form
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Comment/Reply");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // Chuyển đối tượng replyRequest thành chuỗi JSON
            var jsonContent = JsonSerializer.Serialize(replyRequest, options);

            // Thiết lập nội dung yêu cầu dưới dạng StringContent với JSON
            request.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                // Nếu thành công, có thể load lại trang để hiển thị phản hồi mới
                return RedirectToPage();  // Chuyển hướng lại trang để hiển thị bình luận mới
            }

            // Trả về lỗi nếu request thất bại
            return StatusCode((int)response.StatusCode, "Error when sending reply");
        }

        public async Task<IActionResult> OnPostRateAndComment()
        {
            var accessToken = Request.Cookies["accessToken"];
            if (accessToken == null)
            {
                return Unauthorized();
            }

            // Parse JSON request body from the client
            var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
            var rateAndCommentRequest = JsonSerializer.Deserialize<RateAndComment>(requestBody);

            if (rateAndCommentRequest == null)
            {
                return BadRequest("Invalid request data.");
            }

            rateAndCommentRequest.ReviewDate = DateTime.UtcNow;

            // Set up client to send to API
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestContent = new StringContent(JsonSerializer.Serialize(rateAndCommentRequest), Encoding.UTF8, "application/json");

            // Call the external API endpoint for handling the rating and comment
            var response = await client.PostAsync("https://localhost:7126/api/Rate/RateAndComment", requestContent);

            if (response.IsSuccessStatusCode)
            {
                return new JsonResult(new { success = true, message = "Review submitted successfully." });
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Failed to rate and comment: {errorResponse}");
            }
        }


    }
}
