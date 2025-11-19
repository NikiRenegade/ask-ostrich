import yaml from 'js-yaml';

export const yamlToObject = <T>(yamlText: string): T => {
        return yaml.load(yamlText) as T;
};

export const objectToYaml = <T>(data: T): string => {
        return yaml.dump(data, { indent: 2 });
};
