<template>
  <div>
    <ul id="example-1">
      <li v-for="item in tasks" :key="item.id">
        {{ item.task.name }}
        {{ item.description }}
      </li>
    </ul>
  </div>
</template>

<script>
import axios from "axios";
import configuration from "./configuration";

export default {
  name: "App",
  data() {
    return {
      tasks: undefined,
    };
  },
  created: async function () {
    const response = await axios.get(`${configuration.apiBaseUrl}/api/tasks`);
    this.tasks = response.data;
  },
};
</script>
