package sf.services.cognitive.vision;

import java.net.URI;
import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.client.utils.URIBuilder;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClientBuilder;
import org.apache.http.util.EntityUtils;
import org.json.JSONObject;

public class Main {
	// *******************************************************************************************
    // *** Analyze remote pictures with REST-API for Computer Vision of the Cognitive Services ***
    // *******************************************************************************************

    // TODO: (1) IMPORTANT! Replace <API-Key> with your valid subscription key.
    private static final String subscriptionKey = "<API-Key>";


    private static final String urlCommon = ".api.cognitive.microsoft.com";
    private static final String urlVisionCommon = "/vision/v2.0/";
    private static final String urlVisionAnalyze = "analyze";

    // You must use the same Azure region in your REST API method as you used to
    // get your subscription keys. For example, if you got your subscription keys
    // from the East US region, replace "westcentralus" in the urlAzureRegion-Variable
    // below with "eastus".
    //
    // Free trial subscription keys are generated in the "westus" region.
    // If you use a free trial subscription key, you shouldn't need to change
    // this region.
    
    // TODO: (2) assign actual azure region here:
    private static final String urlAzureRegion = "westcentralus"; 
    // private static final String urlAzureRegion = "eastus";
    // ... etc.

    private static final String urlBase = "https://" + urlAzureRegion + urlCommon + urlVisionCommon + urlVisionAnalyze;
    
    // TODO: (3) assign image URL here:
    private static final String imageToAnalyze =
    		"https://cdn.vox-cdn.com/thumbor/hxqCmIJrA8bem6ihwB_nOdZlZB0=/0x0:4000x2667/1200x800/" +
    	    "filters:focal(1650x1744:2290x2384)/cdn.vox-cdn.com/uploads/chorus_image/image/61008591/" +
    		"curbed_new_york_city_street.0.jpg";


    public static void main(String[] args) {
        CloseableHttpClient httpClient = HttpClientBuilder.create().build();

        try {
            URIBuilder builder = new URIBuilder(urlBase);

            // Request parameters. All of them are optional.
            builder.setParameter("visualFeatures", "Categories,Description,Color");
            builder.setParameter("language", "en");

            // Prepare the url for the REST API method.
            URI url = builder.build();
            HttpPost request = new HttpPost(url);

            // Request headers.
            request.setHeader("Content-Type", "application/json");
            request.setHeader("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request body.
            StringEntity requestEntity =
                    new StringEntity("{\"url\":\"" + imageToAnalyze + "\"}");
            request.setEntity(requestEntity);

            // Call the REST API method and get the response entity.
            HttpResponse response = httpClient.execute(request);
            HttpEntity entity = response.getEntity();

            if (entity != null) {
                // Format and display the JSON response.
                String jsonString = EntityUtils.toString(entity);
                JSONObject json = new JSONObject(jsonString);
                System.out.println("REST Response:\n");
                System.out.println(json.toString(2));
            }
        } catch (Exception e) {
            // Display error message.
            System.out.println(e.getMessage());
        }
    }
}
