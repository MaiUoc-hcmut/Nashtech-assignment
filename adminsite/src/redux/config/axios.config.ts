import axios/*, { AxiosResponse }*/ from 'axios';
// import { jwtDecode } from 'jwt-decode';


const instance = axios.create({
    baseURL: "http://localhost:5113/api",
    headers: {
        'Content-Type': 'application/json',
    },  
    withCredentials: true,
});


instance.interceptors.request.use(
    (config) => {
        if (config && config.url) {
            if (
                config.url.includes("/Product") ||
                config.url.includes("/Variant")
            ) {
                config.headers['Content-Type'] = 'multipart/form-data';
                config.headers['Accept'] = 'multipart/form-data';
            } else {
                config.headers['Content-Type'] = 'application/json';
                config.headers['Accept'] = 'application/json';
            }
        }

        return config;
    }, 
    (error) => {
        return Promise.reject(error);
    }
);


export default instance;