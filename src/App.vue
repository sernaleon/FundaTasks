<template>
  <div>{{ tasks }}</div>
</template>

<script>
import axios from 'axios'
import configuration from './configuration'

export default {
  name: "App",
  data() {
    return {
      tasks: "loading...",
    };
  },
  created: async function() {
    const response = await axios.get(`${configuration.apiBaseUrl}/api/tasks`);
    this.tasks = response.data.text;
  }
};
</script>
