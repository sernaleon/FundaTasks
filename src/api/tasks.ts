import { msalPluginInstance } from "@/plugins/msal-plugin";
import axios, { AxiosRequestConfig } from "axios";

export interface TaskType {
    id: string;
    name: string;
}

export interface TaskLineItem {
    id: string;
    task: TaskType;
    description: string;
    timestamp: string;
}

export interface NewTask {
    id?: string;
    name: string;
    description: string;
}

class TasksApi {
    async getTasksAsync(): Promise<TaskLineItem[]> {
        try {
            const config = await this.getAuthorisedConfigAsync();
            const response = await axios.get('/api/tasks', config);

            return response.data;
        } catch (e) {
            console.error(e);
            return [];
        }
    }
    
    async addTaskAsync(item: NewTask): Promise<TaskLineItem[]> {
        try {
            console.log(item);
            const config = await this.getAuthorisedConfigAsync();
            const response = await axios.post('/api/tasks', item, config);

            return response.data;

        } catch (e) {
            console.error(e);
            return [];
        }
    }

    private async getAuthorisedConfigAsync(): Promise<AxiosRequestConfig>{
        const accessToken = await msalPluginInstance.acquireToken();

        const config = {
            headers: { 'Authorization': 'Bearer ' + accessToken }
        };

        return config;
    }
}

export default new TasksApi();